export interface TemplateDimension {
    /**
     * Entity Identity
     */
    templateDimensionId: number;
    /**
     * Id of the containing template
     */
    templateId: number;
    /**
     * Id of the referenced dimension
     */
    dimensionId: number;
    /**
     * Name of the referenced dimension
     */
    dimensionName: string;
    /**
     * Create/Update date of the dimension
     */
    templateDimensionDate: Date;
    /**
     * Create/Update UserID
     */
    userId: number;
    /**
     * Create/Update user name
     */
    userName: string;
    /**
     * Position of the column in the template
     */
    templateDimensionOrder: number;
    /**
     *  Name of the column in the themplate
     */
    templateDimensionName: string;
    /**
     * Flag indicating if the column exists in the database
     */
    isNew: boolean;
    /**
     * Flag indicating if the column data has changed
     */
    isUpdated: boolean;
}
