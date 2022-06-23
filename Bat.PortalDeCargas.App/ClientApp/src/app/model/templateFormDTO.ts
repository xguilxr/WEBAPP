import {TemplateDimensionFormDTO} from './templateDimensionFormDTO';

export interface TemplateFormDTO {
    blobUrl: string;
    description: string;
    endUploadWindow: number;
    fileFormat: number;
    id: number;
    name: string;
    notificationEmail: string;
    notificationText: string;
    periodicity: number;
    updateFeatures: string;
    validation: string;
    dimensions: TemplateDimensionFormDTO[];
    deleted: number[];
}
