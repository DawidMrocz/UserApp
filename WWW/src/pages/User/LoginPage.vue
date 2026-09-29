<template>
  <q-page padding class="flex flex-center">
    <q-card class="q-pa-md" style="width: 350px">
      <q-inner-loading :showing="loading">
        <q-spinner-gears size="50px" color="primary" />
      </q-inner-loading>
      <q-tabs v-model="tab" class="text-center" inline-label>
        <q-tab name="login" label="Zaloguj się" />
        <q-tab name="register" label="Zarejestruj się" />
      </q-tabs>
      <q-separator />

      <div v-if="tab === 'login'">
        <q-form @submit.prevent="onLoginSubmit">
          <q-card-section>
            <q-input
              outlined
              required
              label="Email"
              type="email"
              v-model="loginRequest.email"
              :rules="[emailRule(), requireRule()]"
              clearable
            >
              <template v-slot:append>
                <q-icon name="email" />
              </template>
            </q-input>
            <q-input
              outlined
              required
              label="Hasło"
              :type="isPasswordVisible ? 'text' : 'password'"
              v-model="loginRequest.password"
              class="q-mb-md"
            >
              <template v-slot:append>
                <q-icon
                  :name="isPasswordVisible ? 'visibility' : 'visibility_off'"
                  @click="toggleVisibility"
                />
              </template>
            </q-input>
            <q-checkbox
              v-model="loginRequest.rememberMe"
              label="Nie wylogowuj mnie"
              color="primary"
              keep-color
              class="q-mb-md"
            /><br />
            <div
              class="text-primary"
              style="cursor: pointer"
              @click="navigateToRemind"
            >
              Nie pamiętam hasła
            </div>
          </q-card-section>

          <div class="text-center">
            <q-btn label="Zaloguj" type="submit" color="primary" />
          </div>
        </q-form>
      </div>

      <div v-else>
        <q-form @submit.prevent="onRegisterSubmit">
          <q-card-section>
            <q-input
              v-model="registerRequest.firstName"
              label="Imię"
              outlined
              required
              class="q-mb-md"
            />
            <q-input
              v-model="registerRequest.lastName"
              label="Nazwisko"
              outlined
              required
              class="q-mb-md"
            />
            <q-input
              outlined
              required
              label="Email"
              type="email"
              v-model="registerRequest.email"
              :rules="[emailRule(), requireRule()]"
              clearable
            >
              <template v-slot:append>
                <q-icon name="email" />
              </template>
            </q-input>
            <q-input
              outlined
              required
              label="Hasło"
              :type="isPasswordVisible ? 'text' : 'password'"
              v-model="registerRequest.password"
              class="q-mb-md"
            >
              <template v-slot:append>
                <q-icon
                  :name="isPasswordVisible ? 'visibility' : 'visibility_off'"
                  @click="toggleVisibility"
                />
              </template>
            </q-input>
            <q-input
              outlined
              required
              label="Powtórz hasło"
              :type="isPasswordVisible ? 'text' : 'password'"
              v-model="registerRequest.confirmPassword"
              :rules="[
                passwordRepeatRule(registerRequest.password),
                requireRule(),
              ]"
              class="q-mb-md"
            >
              <template v-slot:append>
                <q-icon
                  :name="isPasswordVisible ? 'visibility' : 'visibility_off'"
                  @click="toggleVisibility"
                />
              </template>
            </q-input>
            <div class="q-mb-md password-requirements">
              <q-list dense>
                <q-item
                  v-for="req in passwordRequirements"
                  :key="req.label"
                  class="q-pa-none"
                >
                  <q-item-section avatar>
                    <q-icon
                      :name="req.fulfilled ? 'check_circle' : 'cancel'"
                      :color="req.fulfilled ? 'green' : 'grey'"
                    />
                  </q-item-section>
                  <q-item-section>
                    <q-item-label :class="{ 'text-green': req.fulfilled }">
                      {{ req.label }}
                    </q-item-label>
                  </q-item-section>
                </q-item>
              </q-list>
            </div>
            <div class="text-center">
              <q-btn label="Zarejestruj" type="submit" color="primary" />
            </div>
          </q-card-section>
        </q-form>
      </div>
    </q-card>
  </q-page>
  <q-dialog v-model="isDialogOpen" persistent>
    <q-card>
      <q-card-section class="row items-center">
        <q-icon name="mail" size="2em" class="q-mr-md" />
        <div>
          <h6 class="q-mb-none">Rejestracja zakończona!</h6>
          <p>Na podany adres e-mail został wysłany link aktywacyjny.</p>
        </div>
      </q-card-section>
      <q-card-actions align="right">
        <q-btn flat label="OK" color="primary" @click="closeDialog" />
      </q-card-actions>
    </q-card>
  </q-dialog>
</template>

<script setup lang="ts">
import {
  UserService,
  LoginRequest,
  RegisterRequest,
} from 'src/api/UserService';
import { ref, watch, computed } from 'vue';
import useAppErrorHandler from 'src/composables/useAppErrorHandler';
import useAppValidation from 'src/composables/useAppValidation';
import { useRouter } from 'vue-router';
import { useUserStore } from 'src/stores/user';
import { Notify } from 'quasar';

const tab = ref<string>('login');
const loading = ref<boolean>(false);
const isPasswordVisible = ref<boolean>(false);
const loginRequest = ref<LoginRequest>({
  email: '',
  password: '',
  rememberMe: false,
});

const registerRequest = ref<RegisterRequest>({
  email: '',
  password: '',
  confirmPassword: '',
  firstName: '',
  lastName: '',
});

const userStore = useUserStore();
const isDialogOpen = ref(false);
const $router = useRouter();
const { errorHandler } = useAppErrorHandler();
const { requireRule, emailRule, passwordRepeatRule } = useAppValidation();

const closeDialog = () => {
  isDialogOpen.value = false;
  tab.value = 'login';
};

const resetForm = () => {
  registerRequest.value = {};
  passwordRequirements.value.forEach((req) => {
    req.fulfilled = false;
  });
};

const passwordRequirements = ref([
  {
    rule: (val) => val.length >= 8,
    label: 'Minimum 8 znaków',
    fulfilled: false,
  },
  {
    rule: (val) => /[A-Z]/.test(val),
    label: 'Przynajmniej jedna wielka litera',
    fulfilled: false,
  },
  {
    rule: (val) => /[a-z]/.test(val),
    label: 'Przynajmniej jedna mała litera',
    fulfilled: false,
  },
  {
    rule: (val) => /[0-9]/.test(val),
    label: 'Przynajmniej jedna cyfra',
    fulfilled: false,
  },
]);

const validatePassword = () => {
  passwordRequirements.value.forEach((req) => {
    req.fulfilled = req.rule(registerRequest.value.password);
  });
};

const allRequirementsFulfilled = computed(() => {
  return passwordRequirements.value.every(
    (requirement) => requirement.fulfilled
  );
});

watch(registerRequest.value, validatePassword);

async function toggleVisibility() {
  isPasswordVisible.value = !isPasswordVisible.value;
}

function navigateToRemind() {
  $router.push('/reset');
}

async function onLoginSubmit() {
  loading.value = true;
  try {
    await userStore.login(loginRequest.value);
    $router.push({ name: 'tasksMain' });
  } catch (err) {
    errorHandler(err);
  } finally {
    loading.value = false;
  }
}

async function onRegisterSubmit() {
  if (!allRequirementsFulfilled.value) {
    Notify.create({
      type: 'negative',
      message: 'Wymagania dotyczące hasła są nie spełnione',
    });
    return;
  }
  loading.value = true;
  try {
    await UserService.Register(registerRequest.value);
    isDialogOpen.value = true;
    resetForm();
  } catch (err) {
    errorHandler(err);
  } finally {
    loading.value = false;
  }
}
</script>
