import { useSyncExternalStore } from 'react';

class AlertStore {
  private listeners: Set<() => void> = new Set();
  private _message: string | null = null;
  private _type: 'error' | 'info' | 'success' | null = null;
  private timeout: ReturnType<typeof setTimeout> | null = null;

  get message() {
    return this._message;
  }
  get type() {
    return this._type;
  }

  showAlert = (msg: string, alertType: 'error' | 'info' | 'success' = 'info') => {
    this._message = msg;
    this._type = alertType;
    this.emit();
    if (this.timeout) clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {
      this._message = null;
      this._type = null;
      this.emit();
    }, 4000);
  };

  subscribe = (listener: () => void) => {
    this.listeners.add(listener);
    return () => this.listeners.delete(listener);
  };

  emit = () => {
    this.listeners.forEach(fn => fn());
  };
}

const alertStore = new AlertStore();

export function useAlertStore() {
  const message = useSyncExternalStore(alertStore.subscribe, () => alertStore.message);
  const type = useSyncExternalStore(alertStore.subscribe, () => alertStore.type);

  return {
    message,
    type,
    showAlert: alertStore.showAlert,
  };
}
