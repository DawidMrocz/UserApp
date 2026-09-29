<template>
  <q-card class="q-mb-md">
    <q-card-section>
      <div class="text-h6">Filtry</div>
    </q-card-section>
    <q-card-section>
      <q-form @submit.prevent="onSubmit">
        <div class="row q-col-gutter-md">
          <div class="col-12 col-md-4">
            <q-input
              filled
              type="text"
              v-model="model.title"
              placeholder="Wprowadź nazwę"
              label="Tytuł"
              @input="onSubmit"
              dense
              clearable
            />
          </div>

          <div class="col-12 col-md-4">
            <q-input
              filled
              v-model="model.dateFrom"
              label="Data od"
              @input="onSubmit"
              dense
              clearable
              type="date"
              placeholder="Wprowadź datę"
            />
          </div>

          <div class="col-12 col-md-4">
            <q-input
              filled
              v-model="model.dateTo"
              label="Data do"
              @input="onSubmit"
              dense
              type="date"
              placeholder="Wprowadź datę"
            />
          </div>

          <div class="col-12 col-md-4">
            <q-select
              v-model="model.statusStrongName"
              :options="StrongNameOptions"
              label="Status"
              emit-value
              map-options
              dense
              class="q-mb-md"
            />
          </div>
        </div>

        <div class="q-mt-md">
          <q-btn color="primary" glossy label="Szukaj" type="submit" />
          <q-btn
            color="secondary"
            label="Wyczyść filtry"
            @click="onClear"
            class="q-ml-md"
            glossy
          />
        </div>
      </q-form>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import {
  TaskSearchRequest,
  TaskStatus,
  TaskService,
} from 'src/api/TaskService';
import { ref, onBeforeMount } from 'vue';
import useAppErrorHandler from 'src/composables/useAppErrorHandler';
const { errorHandler } = useAppErrorHandler();

const StrongNameOptions = ref<TaskStatus[]>([]);

const $emit = defineEmits<{
  (e: 'submit', val: Partial<TaskSearchRequest>): void;
}>();

const model = defineModel<Partial<TaskSearchRequest>>();

function onSubmit(): void {
  $emit('submit');
}

function onClear(): void {
  model.value = {};
  onSubmit();
}

onBeforeMount(async () => {
  try {
    const TaskStatuses = await TaskService.GetTaskStatuses();
    StrongNameOptions.value = TaskStatuses.map((option) => ({
      label: option.name,
      value: option.strongName,
    }));
  } catch (err) {
    errorHandler(err);
  }
});
</script>
