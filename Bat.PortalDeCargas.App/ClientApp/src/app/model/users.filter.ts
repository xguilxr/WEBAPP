import { PaginatedFilter } from './paginatedFilter';

export interface UsersFilter extends PaginatedFilter {
    name?: string;    
}