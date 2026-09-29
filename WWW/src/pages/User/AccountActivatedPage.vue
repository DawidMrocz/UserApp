<template>
  <q-page class="flex flex-center" style="min-width: 380px">
    <q-card>
      <template v-if="loading.value">
        <q-inner-loading>
          <q-spinner-gears size="50px" color="primary" />
        </q-inner-loading>
      </template>
      <template v-else>
        <template v-if="!isError">
          <q-card-section>
            <div class="text-h6 text-center">
              Twoje konto zostało zaktualizowane!
            </div>
          </q-card-section>

          <q-card-section class="flex flex-center">
            <q-icon
              name="sentiment_very_satisfied"
              size="56px"
              color="positive"
              class="happy-icon"
            />
          </q-card-section>

          <q-card-section class="flex flex-center">
            <q-btn
              color="primary"
              label="Wróć do strony głównej"
              @click="goToHome"
              flat
            />
          </q-card-section>
        </template>
        <template v-else>
          <q-card class="q-pa-md text-center">
            <q-card-section>
              <q-icon name="warning" size="64px" color="warning" />
              <div class="text-h6 q-mb-md">
                Link jest niepoprawny lub wygasł
              </div>
            </q-card-section>
            <q-card-actions align="center">
              <q-btn
                label="Powrót do strony głównej"
                color="primary"
                @click="goToHome"
              />
            </q-card-actions>
          </q-card>
        </template>
      </template>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref, onBeforeMount } from 'vue';
import useAppErrorHandler from 'src/composables/useAppErrorHandler';
import { useRouter, useRoute } from 'vue-router';
import { UserService, ActivateRequest } from 'src/api/UserService';

const loading = ref<boolean>(true);
const isError = ref<boolean>(false);
const $router = useRouter();
const route = useRoute();
const { errorHandler } = useAppErrorHandler();

function goToHome() {
  $router.push('/login'); // Przekierowanie do strony głównej
}

onBeforeMount(async () => {
  loading.value = true;
  try {
    const request: ActivateRequest = {
      token: route.params.code,
    };
    await UserService.Activate(request);
  } catch (err) {
    errorHandler(err);
    isError.value = false;
  } finally {
    loading.value = true;
  }
});
</script>

<style scoped>
.q-page {
  max-width: 300px;
  margin: auto;
}

.happy-icon {
  animation: bounce 1s infinite;
}

@keyframes bounce {
  0%,
  20%,
  50%,
  80%,
  100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-10px);
  }
  60% {
    transform: translateY(-5px);
  }
}
</style>
