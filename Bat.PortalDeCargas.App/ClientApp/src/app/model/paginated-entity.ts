export interface PaginatedEntity<T> {
    items: T[];
    currentPage: number;
    itemsPerPage: number;
    totalOfPages: number;
    totalOfItems: number;
}
