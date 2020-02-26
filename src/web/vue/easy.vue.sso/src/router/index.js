import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

import Login from '../components/Login'
import Login2 from '../components/Login2'
import Register from '../components/Register'

let router = new Router(
    {
        mode: 'history',
        routes: [
            { path: '/', redirect: '/account/login' },
            { path: '/account/login', component: Login },
            { path: '/account/login2', component: Login2 },
            { path: '/account/register', component: Register }
        ]
    }
)

export default router