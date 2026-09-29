import { boot } from 'quasar/wrappers';
import { useUserStore } from 'src/stores/user';

export default boot(async () => {
  try {
    const userStore = useUserStore();
    await userStore.loadUser();
  } catch {}
});
