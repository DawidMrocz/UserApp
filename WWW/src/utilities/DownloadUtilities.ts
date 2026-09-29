import { AxiosResponse } from 'axios';

export default {
  extractFilename(response: AxiosResponse): string | undefined {
    const contentDisposition = response.headers['content-disposition'];
    if (contentDisposition === undefined) {
      return;
    }

    const utf8Filename = /filename\*=UTF-8''(.*);?/.exec(contentDisposition);
    if (utf8Filename && utf8Filename?.length > 1) {
      return decodeURIComponent(utf8Filename[1]);
    }

    const filename = /filename="(.*)"/.exec(contentDisposition);
    if (filename && filename?.length > 1) {
      return filename[1];
    }
  },

  downloadFile(response: AxiosResponse<Blob>): void {
    const blob = new Blob([response.data]);
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', this.extractFilename(response) || 'file');
    document.body.appendChild(link);
    link.click();
    URL.revokeObjectURL(url);
  },
};
