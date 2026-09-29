import _ from 'lodash';
import { defineStore } from 'pinia';
import {
  UserService,
  GetUserInfoResponse,
  LoginRequest,
} from 'src/api/UserService';
import { ref } from 'vue';

export const useUserStore = defineStore('user', () => {
  const user = ref<GetUserInfoResponse>();

  async function login(request: LoginRequest): Promise<void> {
    await UserService.Login(request);
    user.value = await UserService.Profile();
  }
  async function logout(): Promise<void> {
    user.value = undefined;
    await UserService.Logout();
  }

  async function loadUser(): Promise<void> {
    user.value = await UserService.Profile();
  }
  function isLoggedIn(): boolean {
    return !_.isNil(user.value);
  }

  return {
    user,
    login,
    logout,
    loadUser,
    isLoggedIn,
  };
});
