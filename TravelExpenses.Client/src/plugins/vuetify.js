import Vue from 'vue'
import Vuetify from 'vuetify'
import Vuelidate from 'vuelidate'
import 'vuetify/dist/vuetify.min.css'
import colors from 'vuetify/es5/util/colors'

const env = process.env.VUE_APP_ENVIRONMENT || 'Development'

if (env === 'Development') {
  document.title = `DEV - ${document.title}`
}

Vue.use(Vuetify, {
  options: {
    customProperties: true
  },
  theme: {
    primary: colors.lightBlue,
    secondary: colors.lightBlue.darken3,
    accent: colors.lightBlue.lighten3
  }
})

Vue.use(Vuelidate)
