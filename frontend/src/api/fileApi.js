import api from './index';

/**
 * Загружает файл игры на сервер.
 * @param {string} gameId - ID игры.
 * @param {File} file - Файл для загрузки.
 * @param {function} onUploadProgress - Колбэк для отслеживания прогресса.
 * @returns {Promise<any>}
 */
export const uploadGameFile = async (gameId, file, onUploadProgress) => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await api.post(`/gamefile/${gameId}`, formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
    onUploadProgress,
  });
  return response.data;
};

/**
 * Скачивает файл игры с сервера.
 * @param {string} gameId - ID игры.
 * @returns {Promise<{blob: Blob, fileName: string}>}
 */
export const downloadGameFile = async (gameId) => {
  const response = await api.get(`/gamefile/${gameId}`, {
    responseType: 'blob',
  });
  
  const contentDisposition = response.headers['content-disposition'];
  let fileName = 'downloaded-file';
  if (contentDisposition) {
    const fileNameMatch = contentDisposition.match(/filename="?(.+)"?/);
    if (fileNameMatch && fileNameMatch.length > 1) {
      fileName = fileNameMatch[1];
    }
  }

  return {
    blob: response.data,
    fileName: fileName
  };
}; 