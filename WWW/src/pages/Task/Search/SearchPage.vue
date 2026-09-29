<template>
  <q-page padding>
    <div class="q-pa-md">
      <q-btn label="Moja tablica" color="primary" :to="`/bilboard`" />
    </div>
    <div class="q-pa-md">
      <q-Task @submit.prevent="onSubmit">
        <TaskSearchFilters v-model="filter" @submit="onSubmit" />
      </q-Task>
      <q-table
        v-model:pagination="pagination"
        title="Zadania"
        :rows="rows"
        :columns="columns"
        row-key="name"
      >
        <template v-slot:body-cell-collect_email="props">
          <q-td :props="props" align="center">
            <q-icon v-if="props.row.collect_email" name="check" color="green" />
            <q-icon v-else name="close" color="red" />
          </q-td>
        </template>
        <template v-slot:body-cell-collect_created_at="props">
          <q-td :props="props" align="center">
            {{ DateUtilities.FormatDate(props.row.created_at) }}
          </q-td>
        </template>

        <!-- ✅ Kolumna z przyciskiem 'Dodaj do koszyka' -->
        <template v-slot:body-cell-addToBilboard="props">
          <q-td :props="props" align="center">
            <q-btn
              color="green"
              icon="dashboard"
              label="Dodaj"
              size="sm"
              @click="addToBilboard(props.row.id)"
            />
          </q-td>
        </template>

        <template v-slot:body-cell-StrongName="props">
          <q-td :props="props" align="center">
            <q-chip
              v-if="props.row.StrongName == 'Backlog'"
              dense
              color="green"
              text-color="white"
              icon="edit"
              label="Szkic"
            />
            <q-chip
              v-if="props.row.StrongName === 'InProgress'"
              dense
              color="primary"
              text-color="white"
              icon="hourglass_empty"
              label="W trakcie"
            />
            <q-chip
              v-if="props.row.StrongName === 'Finished'"
              dense
              color="red"
              text-color="white"
              icon="check_circle"
              label="Zakończony"
            />
          </q-td>
        </template>

        <template v-slot:top-right>
          <q-btn
            color="primary"
            icon="add"
            round
            size="sm"
            @click="addNewTask"
            class="q-mb-md"
          />
        </template>

        <template v-slot:body-cell-actions="props">
          <q-td :props="props">
            <q-btn
              color="grey"
              icon="chevron_right"
              @click="goToDetails(props.row.id)"
              dense
              flat
            />
          </q-td>
        </template>
      </q-table>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import TaskSearchFilters from 'pages/Task/Search/_components/SearchFilters.vue';
import { QTableColumn } from 'quasar';
import { onBeforeMount, ref } from 'vue';
import DateUtilities from 'src/utilities/DateUtilities';
import { TaskService, TaskSearchRequest } from 'src/api/TaskService';
import { useRouter } from 'vue-router';
import useFilterPagination from 'src/composables/useFilterPagination';
import { Filter } from 'src/utilities/FilterUtilities';
import useFilter from 'src/composables/useFilter';
import useAppErrorHandler from 'src/composables/useAppErrorHandler';
// import { Format } from 'date-fns';
import pl from 'date-fns/locale/pl';

const { errorHandler } = useAppErrorHandler();

const { storeFilter, loadFilter } = useFilter(Filter.Task);

const { pagination, storePagination, loadPagination } = useFilterPagination(
  Filter.Task,
  {
    orderBy: 'Title',
    page: 1,
    rowsPerPage: 10,
  }
);

const $router = useRouter();
const loading = ref<boolean>(false);
const rows = ref<TaskSearchResponse>([]);
const filter = ref<Partial<TaskSearchRequest>>();

async function refreshTable() {
  loading.value = true;
  try {
    filter.value.orderBy = pagination.value.sortBy;
    filter.value.page = pagination.value.page;
    filter.value.pagesize = pagination.value.rowsPerPage;

    const data = await TaskService.search({
      ...filter.value,
    });
    rows.value = data.rows;
    storePagination();
  } catch (err) {
    errorHandler(err);
  } finally {
    loading.value = false;
  }
}

function addNewTask() {
  $router.push({ name: 'tasksCreate' });
}

function goToDetails(id: string) {
  $router.push({ name: 'tasks', params: { id } });
}

const columns = ref<QTableColumn[]>([
  {
    name: 'title',
    required: true,
    label: 'Tytuł',
    align: 'left',
    field: (row) => row.title,
    Format: (val) => `${val}`,
    sortable: true,
  },
  {
    name: 'deadline',
    label: 'Termin',
    field: 'deadline',
    align: 'center',
    sortable: true,
    headerStyle: 'width: 50px',
    Format: (val) => Format(new Date(val), 'yyyy-MM-dd', { locale: pl }),
  },
  {
    name: 'isImportant',
    label: 'Priorytetowy',
    field: 'isImportant',
    align: 'center',
    sortable: true,
    headerStyle: 'width: 50px',
  },
  {
    name: 'status',
    label: 'Status',
    field: 'status',
    align: 'center',
    sortable: true,
    headerStyle: 'width: 50px',
  },
  {
    name: 'addToBilboard', // ✅ Nowa kolumna
    label: 'Dodaj do tablicy',
    field: 'addToBilboard',
    align: 'center',
    headerStyle: 'width: 150px',
  },
  {
    name: 'actions',
    label: '',
    field: 'actions',
    headerStyle: 'width: 50px',
  },
]);

async function onSubmit(): Promise<void> {
  storeFilter(filter.value);
  await refreshTable();
}

async function addToBilboard(id: string) {
  try {
    loading.value = true;
    await TaskService.AddToBilboard(id);
    onSubmit(filter.value);
  } catch (error) {
    console.error('Błąd podczas zapisywania zadania:', error);
    alert('Wystąpił błąd. Spróbuj ponownie.');
  } finally {
    loading.value = false;
  }
}

onBeforeMount(async () => {
  filter.value = loadFilter();
  loadPagination();
  onSubmit(filter.value);
});
</script>

<style scoped>
/* Stylowanie breadcrumbs */
.q-breadcrumbs {
  padding: 10px;
  border-radius: 5px;
}

.q-breadcrumbs-el {
  color: #007bff;
}

.q-breadcrumbs-el:last-child {
  color: #6c757d;
}
</style>
