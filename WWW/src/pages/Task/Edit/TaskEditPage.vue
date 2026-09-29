<template>
  <q-page class="flex flex-center">
    <q-card class="q-pa-md shadow-2 form-card">
      <q-card-section>
        <div class="text-h6 text-center">Edytuj Zadanie</div>
      </q-card-section>

      <q-form @submit="submitTask" class="q-gutter-md">
        <!-- Tytuł -->
        <q-input
          filled
          v-model="task.title"
          label="Tytuł zadania *"
          lazy-rules
          :rules="[(val) => !!val || 'Tytuł jest wymagany']"
        >
          <template v-slot:prepend>
            <q-icon name="edit" />
          </template>
        </q-input>

        <!-- Deadline -->
        <q-input filled v-model="task.deadline" label="Deadline" type="date">
          <template v-slot:prepend>
            <q-icon name="event" />
          </template>
        </q-input>

        <!-- Czy ważne? (przełącznik) -->
        <q-toggle
          v-model="task.isImportant"
          label="Ważne zadanie"
          color="red"
        />

        <!-- Szacowane godziny -->
        <q-input
          filled
          v-model.number="task.estimatedHours"
          label="Szacowane godziny"
          type="number"
          step="0.5"
        >
          <template v-slot:prepend>
            <q-icon name="timer" />
          </template>
        </q-input>

        <q-btn label="Powrót" flat color="grey" to="/tasks" />
        <q-btn label="Zapisz" type="submit" color="primary" />
      </q-form>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onBeforeMount } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useQuasar } from 'quasar';
import { TaskService, TaskEditRequest } from 'src/api/TaskService';

const $q = useQuasar();
const route = useRoute();
const router = useRouter();

const task = ref<Partial<TaskEditRequest>>({});

async function submitTask() {
  try {
    if (task.value.deadline) {
      task.value.deadline = task.value.deadline.split('T')[0];
    }

    await TaskService.Update(route.params.id, task.value);
    router.push({ name: 'tasks', params: { id: route.params.id } });
  } catch (error) {
    console.error('Błąd podczas zapisywania zadania:', error);
    alert('Wystąpił błąd. Spróbuj ponownie.');
  }
}

onBeforeMount(async () => {
  try {
    var response = await TaskService.get(route.params.id);
    response.deadline = formatDate(response.deadline);
    task.value = response;
  } catch (error) {
    console.error('Błąd pobierania zadania:', error);
    $q.notify({ message: 'Nie udało się pobrać zadania', color: 'negative' });
  }
});

function formatDate(dateString: string): string {
  return dateString.split('T')[0];
}
</script>
