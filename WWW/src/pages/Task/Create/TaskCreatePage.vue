<template>
  <q-page class="flex flex-center q-pa-md">
    <q-card class="q-pa-md shadow-2 form-card">
      <q-card-section>
        <div class="text-h6 text-center">Dodaj Zadanie</div>
      </q-card-section>

      <q-form @submit="submitCreateTask" class="q-gutter-md">
        <!-- Tytuł (wymagany) -->
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
        <q-card-actions align="right">
          <q-btn label="Powrót" flat color="grey" to="/tasks" />
          <q-btn label="Zapisz" type="submit" color="primary" />
        </q-card-actions>
      </q-form>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useQuasar } from 'quasar';
import { TaskService, TaskCreateRequest } from 'src/api/TaskService';

const $q = useQuasar();
const router = useRouter();

const task = ref<Partial<TaskCreateRequest>>({ isImportant: false });

const loading = ref<bool>(true);

async function submitCreateTask() {
  if (!task.value.title) {
    $q.notify({ message: 'Tytuł zadania jest wymagany!', color: 'negative' });
    return;
  }

  try {
    loading.value = true;

    await TaskService.Create(task.value);
    router.push({ name: 'tasksMain' });
  } catch (error) {
    console.error('Błąd podczas zapisywania zadania:', error);
    alert('Wystąpił błąd. Spróbuj ponownie.');
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.form-card {
  width: 100%;
  max-width: 400px;
  border-radius: 12px;
}
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
