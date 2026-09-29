<template>
  <q-btn flat round class="q-mr-sm">
    <q-avatar color="deep-orange" text-color="white">
      <slot>
        {{ avatarText }}
      </slot>
    </q-avatar>
    <q-menu>
      <q-list dense>
        <q-item clickable v-close-popup @click="onLogout">
          <q-item-section avatar>
            <q-avatar>
              <q-icon name="logout" />
            </q-avatar>
          </q-item-section>
          <q-item-section> Wyloguj </q-item-section>
        </q-item>
      </q-list>
    </q-menu>
  </q-btn>
</template>
<script setup lang="ts">
import { Loading } from 'quasar';
import _ from 'lodash';
import { useUserStore } from 'src/stores/user';
import { useRouter } from 'vue-router';
import { computed } from 'vue';

const $router = useRouter();
const userStore = useUserStore();

const avatarText = computed<string>(() => {
  let names: string[] = [];
  if (!_.isNil(userStore.user)) {
    names = [userStore.user?.firstName, userStore.user?.lastName];
  }
  return names.map((name) => name.charAt(0)).join('');
});

async function onLogout() {
  Loading.show();
  try {
    await userStore.logout();
    await $router.push('/login');
  } finally {
    Loading.hide();
  }
}
</script>
