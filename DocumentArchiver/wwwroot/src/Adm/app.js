import Vue from 'vue'
import VModal from 'vue-js-modal'
import Toasted from 'vue-toasted'


import UserManager from './Component/UserManager.vue'

//Extend & reg
Vue.use(VModal, { dialog: true });
Vue.use(Toasted,
    {
        duration: 3333,
        position: 'top-center',
        theme: 'primary',
        iconPack: 'fontawesome'
    });
new Vue({
    el: '#app',
    render: h => h(UserManager)
});