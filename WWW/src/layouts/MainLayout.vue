<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-toolbar-title> Ankieter </q-toolbar-title>
        <q-toggle
          v-model="isDark"
          :checked-icon="'light_mode'"
          :unchecked-icon="'dark_mode'"
          dense
          @click="changeTheme"
          class="q-mx-lg"
        />
        <MainProfile v-if="userStore.isLoggedIn()" />
      </q-toolbar>
    </q-header>

    <q-page-container>
      <div id="main-layout-container">
        <router-view :style="'max-width: ' + containerWidth" />
      </div>
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import MainProfile from './MainProfile.vue';
import { useUserStore } from 'src/stores/user';
import { useQuasar } from 'quasar';
import { ref, computed, onBeforeMount } from 'vue';
import { useRoute } from 'vue-router';

const $q = useQuasar();
const $route = useRoute();
const userStore = useUserStore();
const isDark = ref<boolean>(false);

function changeTheme() {
  if (isDark.value) {
    $q.dark.set(true);
  } else {
    $q.dark.set(false);
  }
  // $q.dark.toggle();
}

const containerWidth = computed<string>(() => {
  const width = $route.meta.containerWidth ?? 'full';
  switch (width) {
    case 'sm':
      return '600px';
    case 'md':
      return '1024px';
    case 'lg':
      return '1440px';
    case 'xl':
      return '1920px';
    case 'full':
      return '100%';
    default:
      return width;
  }
});

onBeforeMount(async () => {
  if (isDark.value) {
    $q.dark.set(true);
  } else {
    $q.dark.set(false);
  }
});

defineOptions({
  name: 'MainLayout',
});
</script>

<style lang="scss">
@media print {
  #main-layout {
    > .q-header,
    > .q-drawer-container {
      display: none;
    }
    > .q-page-container {
      padding-left: 0 !important;
      padding-right: 0 !important;
      padding-top: 0 !important;
    }
    > .q-footer {
      left: 0 !important;
    }
  }
}
</style>
