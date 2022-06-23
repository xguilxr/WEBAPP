import { dimensionDomain } from "./DimensionDomain";

export interface Dimension{
    
    dimensionId : number;
    dimensionName: string;
    dimensionType: number;
    type: string;
    dimensionSize: number;
    dimensionFormat: string;
    createdUserId: number;
    createdDate: string;
    updatedUserId: number;
    updatedDate: string;
    createdUserName: string;
    updatedUserName: string;
    dimensionStartNumber: number;
    dimensionEndNumber: number;
    dimensionDescription: string;
    domains : dimensionDomain[];

}


 
 
   
