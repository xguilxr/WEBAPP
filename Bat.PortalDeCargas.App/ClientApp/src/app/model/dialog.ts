export enum DialogType {
    Info,
    Error,
    Confirm
  }
  
  export interface DialogConfig {
    dialogType: DialogType;
    title?: string;
    content?: string;
    okButtonText?: string;
    cancelButtonText?: string;
  }
  