import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

import Login from '../components/Login'
import Register from '../components/Register'

let router = new Router(
    {
        mode:'history',
        routes: [
            {
                path: '/', redirect: '/account/login'
            },
            {
                path: '/account/login', component: Login
            },
            {
                path:'/account/register',component: Register
            }
        ]
    }
)

export default router