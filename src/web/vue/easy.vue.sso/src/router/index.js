import Vue from 'vue'
import Router from 'vue-router'
Vue.use(Router)

import Login from '../components/Login'
import Register from '../components/Register'

let router = new Router(
    {
        mode: 'history',
        routes: [
            { path: '/account/login', component: Login },
            { path: '/account/register', component: Register }
        ]
    }
)

router.beforeEach((to,from,next)=>{
    console.log(to)
    if(to.path==='/account/register'||to.path==='/account/login') return next()
    return next('/account/login')
})

export default router