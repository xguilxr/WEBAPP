import {FileDetail} from './fileDetail';
import {PaginatedEntity} from './paginated-entity';

export interface UploadLog {
    uploadLogDate: string;
    templateId: number;
    uploadLogId: number;
    templateName: string;
    userId: number;
    userName: string;
    status: boolean;
    totalLines: number;
    totalInvalidLines: number;
    totalValidationMessages: number;
    details: PaginatedEntity<FileDetail>;
}
