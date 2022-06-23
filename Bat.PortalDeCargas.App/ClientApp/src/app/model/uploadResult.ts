import {FileDetail} from './fileDetail';

export interface UploadResult {
    templateName: string;
    userName: string;
    status: boolean;
    totalLines: number;
    totalInvalidLines: number;
    totalValidationMessages: number;
    fileName: string;
    uploadDate: string;
    details: FileDetail[];
}
