import Vue from 'vue'
import Vuetify from 'vuetify'
import Vuelidate from 'vuelidate'
import 'vuetify/dist/vuetify.min.css'
import colors from 'vuetify/es5/util/colors'

const env = process.env.VUE_APP_ENVIRONMENT || 'Development'

Vue.use(Vuetify, {
  theme: {
    primary: env === 'Development' ? colors.blue.lighten2 : '#4527A0',
    secondary: env === 'Development' ? colors.blue.darken3 : '#7B1FA2',
    accent: '#efefef'
  }
})

Vue.use(Vuelidate)
