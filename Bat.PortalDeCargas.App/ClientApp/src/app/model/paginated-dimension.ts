import { Dimension } from './dimension';


export interface PaginatedDimensions {
  items: Dimension[];
  currentPage: number;
  itemsPerPage: number;
  totalOfPages: number;
  totalOfItems: number;
}
