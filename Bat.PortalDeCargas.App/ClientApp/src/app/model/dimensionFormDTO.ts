type Nullable<T> = T | null;

export interface DimensionFormDTO {
    id: number;
    name: string;
    description: string;
    type: number;
    size: Nullable<number>;
    format: Nullable<string>;
    startNumber: Nullable<number>;
    endNumber: Nullable<number>;
}
