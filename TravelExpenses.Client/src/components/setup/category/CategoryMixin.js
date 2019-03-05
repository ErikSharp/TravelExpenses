import { required, minLength, maxLength } from 'vuelidate/lib/validators'
import { LossGain } from '@/common/constants/StringConstants.js'
import SetupWindow from '@/common/enums/SetupWindows.js'
import { mapState, mapGetters } from 'vuex'

const categoryMustNotBeLossGain = value => {
  return value.toLowerCase() !== LossGain.toLowerCase()
}

const categoryMustHaveIconAndColor = value => {
  return value && value.color && value.icon
}

export default {
  methods: {
    baseValidations() {
      const result = {
        categoryName: {
          required,
          minLength: minLength(3),
          maxLength: maxLength(255),
          categoryMustNotBeLossGain
        },
        editCategory: {
          required,
          categoryMustHaveIconAndColor
        }
      }

      return result
    },
    navColorIcon() {
      this.$store
        .dispatch('SetupData/setSetupWindow', SetupWindow.colorIcon)
        .then(() => {
          this.$v.editCategory.$touch()
        })
    },
    cancel() {
      this.$store.dispatch('Category/clearEditCategory')
      this.$emit('cancel')
    }
  },
  computed: {
    ...mapState('Category', ['editCategory']),
    ...mapGetters('Category', ['editHexColor']),
    items() {
      return this.$store.state.Category.categories
    },
    busy() {
      return this.$store.state.Category.addCategoryBusy
    }
  },
  watch: {
    items() {
      this.categoryName = ''
      this.$v.$reset()
    }
  }
}
