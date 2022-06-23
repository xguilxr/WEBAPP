import {Route} from '@angular/router';
import {AuthGuard} from 'app/core/auth/guards/auth.guard';
import {NoAuthGuard} from 'app/core/auth/guards/noAuth.guard';
import {LayoutComponent} from 'app/layout/layout.component';
import {InitialDataResolver} from 'app/app.resolvers';
import {ErrorComponent} from 'app/error/components/generic/error.component';
import {NotFoundComponent} from './error/components/404/not-found.component';
import {NotAuthorizedComponent} from './error/components/not-authorized/not-authorized.componen';

// @formatter:off
/* eslint-disable max-len */
/* eslint-disable @typescript-eslint/explicit-function-return-type */
export const appRoutes: Route[] = [

    // Redirect empty path to '/dashboard'
    {path: '', pathMatch: 'full', redirectTo: 'dashboard'},

    // Redirect signed in user to the '/example'
    //
    // After the user signs in, the sign in page will redirect the user to the 'signed-in-redirect'
    // path. Below is another redirection for that path to redirect the user to the desired
    // location. This is a small convenience to keep all main routes together here on this file.
    {path: 'signed-in-redirect', pathMatch: 'full', redirectTo: 'dashboard'},

    // Auth routes for guests
    {
        path: '',
        canActivate: [NoAuthGuard],
        canActivateChild: [NoAuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            {
                path: 'confirmation-required',
                loadChildren: () => import('app/modules/auth/confirmation-required/confirmation-required.module').then(m => m.AuthConfirmationRequiredModule)
            },
            {
                path: 'forgot-password',
                loadChildren: () => import('app/modules/auth/forgot-password/forgot-password.module').then(m => m.AuthForgotPasswordModule)
            },
            {
                path: 'reset-password',
                loadChildren: () => import('app/modules/auth/reset-password/reset-password.module').then(m => m.AuthResetPasswordModule)
            },
            {
                path: 'sign-in',
                loadChildren: () => import('app/modules/auth/sign-in/sign-in.module').then(m => m.AuthSignInModule)
            }
        ]
    },

    // Auth routes for authenticated users
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            {
                path: 'sign-out',
                loadChildren: () => import('app/modules/auth/sign-out/sign-out.module').then(m => m.AuthSignOutModule)
            }
        ]
    },

    // Landing routes
    {
        path: '',
        component: LayoutComponent,
        data: {
            layout: 'empty'
        },
        children: [
            {
                path: 'home',
                loadChildren: () => import('app/modules/landing/home/home.module').then(m => m.LandingHomeModule)
            },

        ]
    },
    {
        path: 'error',
        component: ErrorComponent
    },
    {
        path: 'error/not-found',
        component: NotFoundComponent
    },
    {
        path: 'error/not-authorized',
        component: NotAuthorizedComponent
    },
    // Admin routes
    {
        path: '',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        component: LayoutComponent,
        resolve: {
            initialData: InitialDataResolver,
        },
        children: [
            //{path: 'example', loadChildren: () => import('app/modules/admin/example/example.module').then(m => m.ExampleModule)},
            {
                path: 'dashboard',
                loadChildren: () => import('app/modules/admin/dashboard/dashboard.module').then(m => m.DashboardModule),
                canLoad: [AuthGuard],
                data: {
                    userRoles: []
                }
            },
            {
                path: 'uploads',
                loadChildren: () => import('app/modules/admin/uploads/uploads.module').then(m => m.UploadsdModule),
                canLoad: [AuthGuard],
                data: {
                    userRoles: []
                }
            },
            {
                path: 'upload/:id',
                loadChildren: () => import('app/modules/admin/uploads/upload/upload.module').then(m => m.UploadModule),
                data: {
                    userRoles: []
                }
            },

            {
                path: 'dimensoes',
                loadChildren: () => import('app/modules/admin/dimensoes/dimensoes.module').then(m => m.DimensoesModule),
                data: {
                    userRoles: ['admin']
                }
            },
            {
                path: 'criarDimensoes',
                loadChildren: () => import('app/modules/admin/dimensoes/criarDimensoes/criarDimensoes.module').then(m => m.CriarDimensoesModule),
                data: {
                    userRoles: ['admin']
                }
            },
            {
                path: 'criarDimensoes/:id',
                loadChildren: () => import('app/modules/admin/dimensoes/criarDimensoes/criarDimensoes.module').then(m => m.CriarDimensoesModule),
                data: {
                    userRoles: ['admin']
                }
            },


            {
                path: 'templates',
                loadChildren: () => import('app/modules/admin/templates/templates.module').then(m => m.TemplatesModule),
                data: {
                    userRoles: ['admin']
                }
            },
            {
                path: 'editTemplateForm',
                loadChildren: () => import('app/modules/admin/templates/editTemplateForm/edit-template-form.module').then(m => m.EditTemplateFormModule),
                data: {
                    userRoles: ['admin']
                }
            },
            {
                path: 'editTemplateForm/:id',
                loadChildren: () => import('app/modules/admin/templates/editTemplateForm/edit-template-form.module').then(m => m.EditTemplateFormModule),
                data: {
                    userRoles: ['admin']
                }
            },
            {
                path: 'gerenciamento',
                loadChildren: () => import('app/modules/admin/gerenciamento/gerenciamento.module').then(m => m.GerenciamentoModule),
                data: {
                    userRoles: ['admin']
                }
            },
             /*{ path: 'carga', loadChildren: () => import('app/modules/admin/carga/carga.module').then(m => m.CargaModule) },*/
            {
                path: 'usuarios',
                loadChildren: () => import('app/modules/admin/usuarios/usuarios.module').then(m => m.UsuariosModule),
                data: {
                    userRoles: ['admin']
                }
            },
            {
                path: 'cadastrousuarios',
                loadChildren: () => import('app/modules/admin/usuarios/cadastrousuarios/cadastrousuarios.module').then(m => m.CadastroUsuariosModule),
                data: {
                    userRoles: ['admin']
                }
            },
            {
                path: 'cadastrousuarios/:id',
                loadChildren: () => import('app/modules/admin/usuarios/cadastrousuarios/cadastrousuarios.module').then(m => m.CadastroUsuariosModule),
                data: {
                    userRoles: ['admin']
                }
            }
        ]
    }
];
