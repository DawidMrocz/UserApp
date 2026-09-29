import { api } from 'src/boot/axios';
import { AxiosResponse } from 'axios';

export interface TaskChangeStatusRequest {
  statusStrongName: string;
}

export interface BillboardItem {
  bilboardItemId: number;
  hours?: number;
  title: string;
  deadline?: string; // ISO 8601 format, np. "2024-02-01T12:00:00Z"
  statusId: number;
  estimatedHours?: number;
  isImportant: boolean;
}

export interface BilboardGetResponse {
  id: number;
  firstName: string;
  lastName: string;
  bilboardItems: BillboardItem[];
}

export const BillboardService = {
  async Get(): Promise<BilboardGetResponse> {
    const response: AxiosResponse<BilboardGetResponse> = await api.get(
      '/bilboards'
    );
    return response.data;
  },

  async Delete(bilboardItemId: number): Promise<void> {
    await api.delete(`/bilboards/${bilboardItemId}`);
  },

  async ChangeStatus(
    bilboardItemId: number,
    request: TaskChangeStatusRequest
  ): Promise<void> {
    await api.put(`/bilboards/${bilboardItemId}/change-status`, request);
  },
};
