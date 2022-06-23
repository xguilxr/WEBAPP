import {TemplateDimension} from './templateDimension';

export interface Template {
    createDate: Date;
    createUserId: number;
    createUserName: string;
    templateBlobUrl: string;
    templateDescription: string;
    templateEndUploadWindow: number;
    templateFileFormat: {
        name: string; value: number;
    };
    templateId: number;
    templateName: string;
    templateNotificationEmail: string;
    templateNotificationText: string;
    templatePeriodicity: number;
    templateUpdateFeatures: string;
    templateValidation: string;
    updateDate: Date | null;
    updateUserId: number | null;
    updateUserName: string | null;
    dimensions: TemplateDimension[];
    deleted: number[];
}
