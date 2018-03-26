import store from './store'

import caseListing from './Component/CaseListingView.vue'
import userManager from './Component/UserManager.vue'
import login from './Component/Login.vue'



function requireAuth(to, from, next) {
    if (!store.getters.IsAuthenticated) {
        next({
            path: '/Login',
            query: { redirect: to.path }
        })
    } else {
        next()
    }
}

const routes = [
    //Default
    {
        path: '/',
        redirect: '/Home'
    },
    //No Auth
    { path: '/Home', name: 'Home', component: caseListing, display: 'Trang chính', navbar: true, beforeEnter: requireAuth },
    { path: '/Adm', name: 'Adm', component: userManager, display: 'Quản lý', navbar: true, beforeEnter: requireAuth },
    { path: '/Info', name: 'Info', component: userManager, display: 'Tài khoản', navbar: true, beforeEnter: requireAuth },
    //Auth
    { path: '/Login', name: 'Login', component: login, display: 'Đăng nhập', navbar: false }];

module.exports = routes;