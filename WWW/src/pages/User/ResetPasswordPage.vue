<template>
  <q-page class="flex flex-center">
    <q-card class="q-pa-md">
      <q-inner-loading :showing="loading">
        <q-spinner-gears size="50px" color="primary" />
      </q-inner-loading>
      <div>
        <q-form @submit.prevent="onPasswordResetSubmit">
          <q-card-section class="bg-primary text-white">
            <div class="text-h6">Przypomnij hasło</div>
          </q-card-section>
          <q-card-section>
            <p>
              Na podany adres e-mail zostanie wysłany link do<br />
              zmiany hasła.
            </p>
          </q-card-section>
          <q-card-section>
            <q-input
              outlined
              required
              label="E-mail"
              type="email"
              v-model="passwordResetRequest.email"
              :rules="[emailRule(), requireRule()]"
              clearable
            >
              <template v-slot:append>
                <q-icon name="email" />
              </template>
            </q-input>
          </q-card-section>

          <div class="text-center">
            <q-btn
              label="Powrót"
              type="button"
              color="secondary"
              @click="navigateToLogin"
              style="margin-right: 10px"
            />
            <q-btn label="Wyślij" type="submit" color="primary" />
          </div>
        </q-form>
      </div>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { UserService, PasswordResetRequest } from 'src/api/UserService';
import { ref } from 'vue';
import useAppErrorHandler from 'src/composables/useAppErrorHandler';
import useAppValidation from 'src/composables/useAppValidation';
import { useRouter } from 'vue-router';

const $router = useRouter();
const { errorHandler } = useAppErrorHandler();
const { requireRule, emailRule } = useAppValidation();
const loading = ref<boolean>(false);
const passwordResetRequest = ref<PasswordResetRequest>({
  email: '',
});

function navigateToLogin() {
  $router.push('/login');
}

async function onPasswordResetSubmit() {
  loading.value = true;
  try {
    await UserService.Reset(passwordResetRequest.value);
  } catch (err) {
    errorHandler(err);
  } finally {
    loading.value = false;
  }
}
</script>
