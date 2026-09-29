import { api } from 'src/boot/axios';

export interface LoginRequest {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
}

export interface Role {
  roleId: number;
  name: string;
  strongName: string;
}

export interface GetUserInfoResponse {
  id: number;
  email: string;
  lastName?: string;
  firstName?: string;
  roles: Role[];
}

export interface LoginResponse {
  CSRFToken: string;
}

export interface RefreshTokenResponse {
  CSRFToken: string;
}

export interface PasswordResetRequest {
  email: string;
}

export interface ActivateRequest {
  token: string;
}

export const UserService = {
  async Login(request: LoginRequest): Promise<LoginResponse> {
    const response = await api.post('user/login', request);

    if (response.data.csrfToken) {
      localStorage.setItem('csrfToken', response.data.csrfToken);
    }

    return response.data;
  },
  async Register(request: RegisterRequest): Promise<void> {
    await api.post('user/register', request);
  },
  async Logout(): Promise<void> {
    await api.post('user/logout');
  },
  async RefreshToken(): Promise<RefreshTokenResponse> {
    const response = await api.post('tokens/refresh');
    return response.data;
  },
  async Profile(): Promise<GetUserInfoResponse> {
    const response = await api.get('user/profile');
    return response.data;
  },
};
