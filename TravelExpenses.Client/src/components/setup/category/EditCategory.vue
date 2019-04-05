<template>
  <div>
    <h2 class="white--text">Edit category</h2>
    <v-text-field
      v-model.trim="categoryName"
      :error-messages="categoryNameErrors"
      label="Edit name"
      box
      background-color="white"
      color="primary"
      @input="$v.categoryName.$touch()"
      @blur="$v.categoryName.$touch()"
    ></v-text-field>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn :color="editHexColor" @click="navColorIcon">
          <v-icon class="white--text">{{
            editCategory ? editCategory.icon : ''
          }}</v-icon>
        </v-btn>
        <v-btn
          dark
          color="primary"
          :disabled="!$v.$anyDirty || $v.$invalid"
          :loading="busy"
          @click="edit"
          >Save</v-btn
        >
        <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import CategoryMixin from '@/components/setup/category/CategoryMixin.js'

const categoryMustBeUniqueOrOriginal = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.categoryName.toLowerCase())

  let isUnique = itemsLowered.indexOf(value.toLowerCase()) < 0
  let isOriginal = vm.originalCategoryName
    ? vm.originalCategoryName == value
    : true
  return isUnique || isOriginal
}

export default {
  mixins: [CategoryMixin],
  data() {
    return {
      originalCategoryName: ''
    }
  },
  validations() {
    const base = this.baseValidations()
    base.categoryName[
      'categoryMustBeUniqueOrOriginal'
    ] = categoryMustBeUniqueOrOriginal

    return base
  },
  methods: {
    edit() {
      this.$store.dispatch('Category/editCategory').then(() => {
        this.originalCategoryName = ''
        this.$emit('cancel')
      })
    }
  },
  computed: {
    categoryName: {
      get() {
        return this.$store.state.Category.editCategory
          ? this.$store.state.Category.editCategory.categoryName
          : ''
      },
      set(val) {
        if (val && !this.originalCategoryName) {
          this.originalCategoryName = this.categoryName
        }

        this.$store.dispatch('Category/setName', val).then(() => {
          if (val && this.originalCategoryName == val) {
            this.$v.categoryName.$reset()
          }
        })
      }
    },
    categoryNameErrors() {
      const errors = []

      if (!this.$v.categoryName.$dirty) return errors

      !this.$v.categoryName.maxLength &&
        errors.push(
          `The category can be a maximum of ${
            this.$v.categoryName.$params.maxLength.max
          } characters`
        )

      !this.$v.categoryName.minLength &&
        errors.push(
          `The category must be a minimum of ${
            this.$v.categoryName.$params.minLength.min
          } characters`
        )

      !this.$v.categoryName.categoryMustBeUniqueOrOriginal &&
        errors.push('The category must be unique or the original value')

      !this.$v.categoryName.categoryMustNotBeLossGain &&
        errors.push(`The category name must not be ${LossGain}`)

      !this.$v.categoryName.required && errors.push('A category is required')
      return errors
    }
  }
}
</script>

<style scoped>
>>> .v-text-field__details {
  margin-bottom: 0 !important;
}
</style>
