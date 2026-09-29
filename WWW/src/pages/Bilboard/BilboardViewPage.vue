<template>
  <q-page class="q-pa-md">
    <q-card class="q-pa-md shadow-2">
      <q-card-section>
        <div class="text-h5">
          Tablica zadań: {{ user?.firstName }} {{ user?.lastName }}
        </div>
      </q-card-section>

      <q-table
        v-if="user && user.bilboardItems.length"
        :rows="user.bilboardItems"
        :columns="columns"
        row-key="bilboardItemId"
        bordered
        flat
        separator="horizontal"
      >
        <template v-slot:body-cell-title="props">
          <q-td>
            <q-icon
              v-if="props.row.isImportant"
              name="priority_high"
              color="red"
              class="q-mr-sm"
            />
            {{ props.row.title }}
          </q-td>
        </template>

        <template v-slot:body-cell-deadline="props">
          <q-td>
            {{ formatDate(props.row.deadline) }}
          </q-td>
        </template>

        <template v-slot:body-cell-status="props">
          <q-td>
            <q-select
              v-model="props.row.status"
              :options="StrongNameOptions"
              dense
              outlined
              emit-value
              map-options
              @update:model-value="changeStatus(props.row)"
            />
          </q-td>
        </template>

        <template v-slot:body-cell-actions="props">
          <q-td>
            <q-btn
              icon="delete"
              color="red"
              flat
              round
              dense
              @click="deleteItem(props.row.bilboardItemId)"
            />
          </q-td>
        </template>
      </q-table>

      <q-card-section v-else>
        <div class="text-center text-grey-8">Brak zadań do wyświetlenia</div>
      </q-card-section>
    </q-card>
    <div class="q-pa-md">
      <q-btn label="Powrót" color="grey" :to="`/tasks`" />
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onBeforeMount } from 'vue';
import { useQuasar, date } from 'quasar';
import { BillboardService } from 'src/api/BilboardService';
import { TaskStatus, TaskService } from 'src/api/TaskService';

const $q = useQuasar();
const user = ref<Partial<BilboardGetResponse>>();
const StrongNameOptions = ref<TaskStatus[]>([]);

const columns = [
  { name: 'title', label: 'Tytuł', align: 'left', field: 'title' },
  { name: 'deadline', label: 'Termin', align: 'center', field: 'deadline' },
  { name: 'status', label: 'Status', align: 'center', field: 'status' },
  { name: 'actions', label: 'Akcje', align: 'center', field: 'actions' },
];

const formatDate = (isoDate) => {
  return isoDate ? date.formatDate(isoDate, 'DD/MM/YYYY HH:mm') : '-';
};

async function loadBillboard() {
  try {
    user.value = await BillboardService.Get();
  } catch (error) {
    console.error('Błąd pobierania zadań:', error);
    $q.notify({
      message: 'Nie udało się pobrać listy zadań',
      color: 'negative',
    });
  }
}

const changeStatus = async (item) => {
  try {
    console.log(item);
    await BillboardService.ChangeStatus(item.bilboardItemId, {
      statusStrongName: item.status,
    });
    $q.notify({ message: 'Status zaktualizowany!', color: 'positive' });
  } catch (error) {
    console.error('Błąd zmiany statusu:', error);
    $q.notify({ message: 'Nie udało się zmienić statusu', color: 'negative' });
  }
};

const deleteItem = async (bilboardItemId) => {
  $q.dialog({
    title: 'Potwierdzenie',
    message: 'Czy na pewno chcesz usunąć to zadanie?',
    cancel: true,
    persistent: true,
  }).onOk(async () => {
    try {
      await BillboardService.Delete(bilboardItemId);
      user.value.bilboardItems = user.value.bilboardItems.filter(
        (item) => item.bilboardItemId !== bilboardItemId
      );
      $q.notify({ message: 'Zadanie usunięte!', color: 'positive' });
    } catch (error) {
      console.error('Błąd usuwania:', error);
      $q.notify({ message: 'Nie udało się usunąć zadania', color: 'negative' });
    }
  });
};

onBeforeMount(async () => {
  const TaskStatuses = await TaskService.GetTaskStatuses();

  StrongNameOptions.value = TaskStatuses.map((option) => ({
    label: option.name,
    value: option.strongName,
  }));

  await loadBillboard();
});
</script>

<style scoped>
.q-table {
  max-width: 800px;
  margin: auto;
}
</style>
