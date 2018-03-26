import Vue from 'vue'
import VueRouter from 'vue-router'

import store from './store'
import routes from './routes'

import caseListing from './Component/CaseListingView.vue'
import userManager from './Component/UserManager.vue'
import login from './Component/Login.vue'

Vue.use(VueRouter)


export default new VueRouter({
    mode: 'history',
    routes: routes
});