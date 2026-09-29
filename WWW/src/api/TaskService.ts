import { api } from 'src/boot/axios';
import { AxiosResponse } from 'axios';

export interface TaskSearchRequest {
  page?: number;
  pageSize?: number;
  orderBy?: string;
  orderDir?: number;
  title?: string;
  dateFrom?: Date;
  dateTo?: Date;
  statusStrongName?: string;
}

export enum TaskStatusEnum {
  Unknown = 0,
  Backlog = 1,
  InProcess = 2,
  Finished = 3,
}

export interface TaskSearchResponse {
  id: number;
  title: string;
  deadline?: string;
  isImportant: boolean;
  estimatedHours?: number;
  status: TaskStatusEnum;
  totalRows: number;
}

export interface FormSearchResult {
  rows: TaskSearchResponse[];
  totalRows: number;
}

export interface TaskGetResponse {
  id: number;
  createUserId: number;
  createDate: string; // ISO 8601 format, np. "2024-02-01T12:00:00Z"
  modifyUserId?: number;
  modifyDate?: string;
  title: string;
  deadline?: string;
  userId?: number;
  isImportant: boolean;
  estimatedHours?: number;
  statusId: number;
  status: TaskStatusEnum;
}

export interface TaskCreateRequest {
  title: string;
  deadline?: string;
  isImportant: boolean;
  estimatedHours?: number;
}

export interface TaskEditRequest extends TaskCreateRequest {}

export interface TaskStatusResponse {
  id: number;
  name: string;
  strongName: string;
}

export interface TaskUpdateRequest extends TaskCreateRequest {}

export const TaskService = {
  async search(data: TaskSearchRequest): Promise<FormSearchResult> {
    const response: AxiosResponse<FormSearchResult> = await api.get('/tasks/', {
      params: data,
    });
    return response.data;
  },
  async get(id: number): Promise<TaskGetResponse> {
    const response: AxiosResponse<TaskGetResponse> = await api.get(
      '/tasks/' + id + '/'
    );
    return response.data;
  },

  async GetDocument(documentGuid: string): Promise<Blob> {
    const response: AxiosResponse<Blob> = await api.get(
      `/tasks/document/${documentGuid}`,
      {
        responseType: 'blob',
      }
    );
    return response.data;
  },

  async CreateDocument(taskId: number, file: File): Promise<void> {
    const formData = new FormData();
    formData.append('file', file);

    await api.post(`/tasks/${taskId}/document`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
  },

  async DeleteDocument(documentGuid: string): Promise<void> {
    await api.delete(`/tasks/document/${documentGuid}`);
  },

  async Create(data: TaskCreateRequest): Promise<number> {
    const response: AxiosResponse<number> = await api.post('/tasks', data);
    return response.data;
  },
  async Update(id: number, data: TaskUpdateRequest): Promise<number> {
    const response: AxiosResponse<number> = await api.put('/tasks/' + id, data);
    return response.data;
  },
  async Delete(id: number): Promise<void> {
    await api.delete('/tasks/' + id + '/');
  },
  async AddToBilboard(id: number): Promise<void> {
    await api.put('/tasks/' + id + '/add-to-bilboard');
  },

  async GetTaskStatuses(): Promise<TaskStatusResponse> {
    const response: AxiosResponse<TaskStatusResponse> = await api.get(
      '/tasks/statuses'
    );
    return response.data;
  },
};
