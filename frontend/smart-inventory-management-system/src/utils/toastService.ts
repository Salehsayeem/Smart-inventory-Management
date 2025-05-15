//utils/toastService.ts
import { toast, ToastOptions } from 'react-toastify';

const toastConfig: ToastOptions = {
  position: "top-right",
  autoClose: 3000,
  hideProgressBar: false,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true,
  progress: undefined,
  theme: "light",
};

export const showSuccess = (message: string) => {
  toast.success(message, toastConfig);
};

export const showError = (message: string) => {
  toast.error(message, toastConfig);
};

export const showWarning = (message: string) => {
  toast.warning(message, toastConfig);
};

export const showInfo = (message: string) => {
  toast.info(message, toastConfig);
};
