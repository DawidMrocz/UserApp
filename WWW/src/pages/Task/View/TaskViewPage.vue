<template>
  <q-page class="flex flex-center q-pa-md">
    <q-card v-if="task" class="q-pa-md shadow-3 task-card">
      <q-card-section>
        <div class="text-h6 text-center">Szczegóły Zadania</div>
      </q-card-section>

      <q-card-section class="q-gutter-md">
        <q-item>
          <q-item-section avatar>
            <q-icon name="title" color="primary" />
          </q-item-section>
          <q-item-section>
            <q-item-label class="text-bold">Tytuł:</q-item-label>
            <q-item-label>{{ task.title }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item>
          <q-item-section avatar>
            <q-icon name="event" color="primary" />
          </q-item-section>
          <q-item-section>
            <q-item-label class="text-bold">Utworzono:</q-item-label>
            <q-item-label>{{ formatDate(task.createDate) }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item v-if="task.modifyDate">
          <q-item-section avatar>
            <q-icon name="event" color="primary" />
          </q-item-section>
          <q-item-section>
            <q-item-label class="text-bold">Zmodyfikowano:</q-item-label>
            <q-item-label>{{ formatDate(task.modifyDate) }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item v-if="task.deadline">
          <q-item-section avatar>
            <q-icon name="event_available" color="red" />
          </q-item-section>
          <q-item-section>
            <q-item-label class="text-bold">Termin:</q-item-label>
            <q-item-label>{{ formatDate(task.deadline) }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item>
          <q-item-section avatar>
            <q-icon
              :name="task.isImportant ? 'priority_high' : 'low_priority'"
              color="red"
            />
          </q-item-section>
          <q-item-section>
            <q-item-label class="text-bold">Priorytet:</q-item-label>
            <q-item-label>{{
              task.isImportant ? 'Ważne' : 'Zwykłe'
            }}</q-item-label>
          </q-item-section>
        </q-item>

        <q-item v-if="task.estimatedHours">
          <q-item-section avatar>
            <q-icon name="access_time" color="blue" />
          </q-item-section>
          <q-item-section>
            <q-item-label class="text-bold">Szacowane godziny:</q-item-label>
            <q-item-label>{{ task.estimatedHours }}h</q-item-label>
          </q-item-section>
        </q-item>

        <q-item>
          <q-item-section avatar>
            <q-icon name="check_circle" :color="getBadgeColor(task.status)" />
          </q-item-section>
          <q-item-section>
            <q-item-label class="text-bold">Status:</q-item-label>
            <q-badge :color="getBadgeColor(task.status)" class="q-mt-xs">
              {{ task.status }}
            </q-badge>
          </q-item-section>
        </q-item>
      </q-card-section>

      <q-separator class="q-my-md" />

      <!-- LISTA PLIKÓW -->
      <q-card-section v-if="task.files.length">
        <div class="text-h6">Załączniki:</div>
        <q-list bordered separator>
          <q-item v-for="file in task.files" :key="file.id">
            <q-item-section>
              <q-item-label>{{ file.name }}</q-item-label>
            </q-item-section>
            <q-item-section side>
              <q-btn
                icon="cloud_download"
                color="primary"
                flat
                @click="downloadDocument(file.guid)"
              />
              <q-btn
                icon="delete"
                color="red"
                flat
                @click="deleteDocument(file.guid)"
              />
            </q-item-section>
          </q-item>
        </q-list>
      </q-card-section>

      <q-separator class="q-my-md" />

      <!-- FORMULARZ DODAWANIA PLIKU -->
      <q-form @submit="submitFile" class="q-gutter-md">
        <q-file
          filled
          v-model="selectedFile"
          label="Załącz plik"
          accept=".jpg, .png, .pdf, .docx"
          clearable
        >
          <template v-slot:prepend>
            <q-icon name="attach_file" />
          </template>
        </q-file>

        <q-btn label="Dodaj plik" type="submit" color="secondary" />
      </q-form>

      <q-card-actions align="right">
        <q-btn label="Powrót" flat color="grey" to="/tasks" />
        <q-btn label="Edytuj" color="primary" :to="`/tasks/edit/${task.id}`" />
      </q-card-actions>
    </q-card>

    <q-spinner v-else color="primary" size="50px" />
  </q-page>
</template>

<script setup lang="ts">
import { ref, onBeforeMount } from 'vue';
import { useRoute } from 'vue-router';
import { date, useQuasar } from 'quasar';
import { TaskService } from 'src/api/TaskService';

const route = useRoute();
const $q = useQuasar();
const task = ref();

const selectedFile = ref(null);

const formatDate = (isoDate) => {
  return date.formatDate(isoDate, 'DD/MM/YYYY HH:mm');
};

function getBadgeColor(status) {
  switch (status) {
    case 'Nowy':
      return 'blue';
    case 'W toku':
      return 'yellow';
    case 'Zakończony':
      return 'green';
    default:
      return 'red';
  }
}

async function submitFile() {
  console.log(route.params.id);
  if (!selectedFile.value) return;

  try {
    await TaskService.CreateDocument(route.params.id, selectedFile.value);
    $q.notify({ message: 'Plik przesłany!', color: 'positive' });
    selectedFile.value = null;
    await load();
  } catch (error) {
    console.error('Błąd przesyłania pliku:', error);
    $q.notify({ message: 'Nie udało się przesłać pliku', color: 'negative' });
  }
}

async function downloadDocument(documentGuid: string) {
  try {
    const fileBlob = await TaskService.GetDocument(documentGuid);
    const url = window.URL.createObjectURL(fileBlob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'plik';
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
  } catch (error) {
    console.error('Błąd pobierania pliku:', error);
    $q.notify({ message: 'Nie udało się pobrać pliku', color: 'negative' });
  }
}

async function deleteDocument(documentGuid: string) {
  try {
    await TaskService.DeleteDocument(documentGuid);
    $q.notify({ message: 'Plik usunięty!', color: 'positive' });
    await load();
  } catch (error) {
    console.error('Błąd usuwania pliku:');
    $q.notify({ message: 'Nie udało się usunąć pliku', color: 'negative' });
  }
}

async function load() {
  task.value = await TaskService.get(route.params.id);
}

onBeforeMount(async () => {
  try {
    await load();
  } catch (error) {
    console.error('Błąd pobierania zadania:', error);
    $q.notify({ message: 'Nie udało się pobrać zadania', color: 'negative' });
  }
});
</script>

<style scoped>
.task-card {
  width: 100%;
  max-width: 450px;
  border-radius: 12px;
}
.text-bold {
  font-weight: bold;
}
</style>
