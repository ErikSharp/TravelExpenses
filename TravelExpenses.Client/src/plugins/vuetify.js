import Vue from 'vue'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import colors from 'vuetify/es5/util/colors'

const env = process.env.VUE_APP_ENVIRONMENT || 'Development'

Vue.use(Vuetify, {
  theme: {
    primary:
      env === 'Development' ? colors.orange.lighten2 : colors.blue.lighten2,
    secondary:
      env === 'Development' ? colors.orange.darken3 : colors.blue.darken3,
    accent: '#efefef'
  }
})
