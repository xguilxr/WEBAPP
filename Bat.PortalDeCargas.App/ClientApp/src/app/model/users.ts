import { UserType } from "./user.type";

export interface AppUser{
    userId :number;
    userName:string;
    userEmail:string;
    userType:UserType;  
    password:string;
    confirmPassword : string;    
}

export interface ChangePassword{
    userId :number;
    password:string;
    confirmPassword : string;   
}