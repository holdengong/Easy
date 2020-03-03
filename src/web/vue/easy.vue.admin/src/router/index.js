import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

import Home from '../components/Home'
import Users from '../components/Users'
import Menus from '../components/Menus'
import Roles from '../components/Roles'

let router = new Router(
    {
        mode: 'history',
        routes: [
            {
                path: '/', component: Home, children: [
                    { path: '/users', component: Users },
                    {path:'/menus',component:Menus},
                    {path:'roles',component:Roles}
                ]
            },
        ]
    }
)

router.beforeEach((to, from, next) => {
    return next()
})

export default router