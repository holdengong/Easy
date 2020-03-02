import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

import Home from '../components/Home'
import Users from '../components/Users'
import Permissions from '../components/Permissions'

let router = new Router(
    {
        mode: 'history',
        routes: [
            {
                path: '/', component: Home, children: [
                    { path: '/users', component: Users },
                    {path:'/permissions',component:Permissions}
                ]
            },
        ]
    }
)

router.beforeEach((to, from, next) => {
    return next()
})

export default router