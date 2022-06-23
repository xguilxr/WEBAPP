export interface Paginated<Type> {
    items: Type[];
    currentPage: number;
    itemsPerPage: number;
    totalOfPages: number;
    totalOfItems: number;
  }