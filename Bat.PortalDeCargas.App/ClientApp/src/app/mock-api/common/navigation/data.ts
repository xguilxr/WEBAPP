/* tslint:disable:max-line-length */
import {FuseNavigationItem} from '@fuse/components/navigation';

export const defaultNavigation: FuseNavigationItem[] = [
    {
        id: 'dashboard',
        title: 'dashboard',
        type: 'basic',
        link: '/dashboard',
        roles: ['regular', 'admin']
    }, {
        id: 'uploads',
        title: 'uploads',
        type: 'basic',
        link: '/uploads',
        roles: ['regular', 'admin']
    }, {
        id: 'dimensoes',
        title: 'dimensoes',
        type: 'basic',
        link: '/dimensoes',
        roles: ['admin']
    }, {
        id: 'templates',
        title: 'templates',
        type: 'basic',
        link: '/templates',
        roles: ['admin']
    }, {
        id: 'gerenciamento',
        title: 'gerenciamento',
        type: 'basic',
        link: '/gerenciamento',
        roles: ['admin']
    }, {
        id: 'carga',
        title: 'carga',
        type: 'basic',
        link: '/carga',
        roles: ['admin']
    }, {
        id: 'usuarios',
        title: 'usuarios',
        type: 'basic',
        link: '/usuarios',
        roles: ['admin']
    }];
export const compactNavigation: FuseNavigationItem[] = defaultNavigation;
export const futuristicNavigation: FuseNavigationItem[] = defaultNavigation;
export const horizontalNavigation: FuseNavigationItem[] = defaultNavigation;
