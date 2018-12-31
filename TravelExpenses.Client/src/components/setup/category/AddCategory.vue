<template>
  <div>
    <h2 class="white--text">Add additional category</h2>
    <v-text-field
      v-model.trim="category"
      :error-messages="categoryErrors"
      label="Enter category name"
      box
      background-color="white"
      color="primary"
      @input="$v.category.$touch()"
      @blur="$v.category.$touch()"
    ></v-text-field>
    <v-container>
      <v-layout row wrap>
        <v-flex xs4 offset-xs2>
          <v-btn dark color="primary" :disabled="$v.$invalid" :loading="busy" @click="add">Add</v-btn>
        </v-flex>
        <v-flex xs4 offset-xs2>
          <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

const categoryMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.categoryName.toLowerCase())
  return itemsLowered.indexOf(value.toLowerCase()) < 0
}

export default {
  validations() {
    const result = {
      category: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255),
        categoryMustBeUnique
      }
    }

    return result
  },
  data() {
    return {
      category: ''
    }
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    add() {
      this.$store.dispatch('Category/addCategory', this.category)
    }
  },
  computed: {
    categoryErrors() {
      const errors = []

      if (!this.$v.category.$dirty) return errors

      !this.$v.category.maxLength &&
        errors.push(
          `The category can be a maximum of ${
            this.$v.category.$params.maxLength.max
          } characters`
        )
      !this.$v.category.minLength &&
        errors.push(
          `The category must be a minimum of ${
            this.$v.category.$params.minLength.min
          } characters`
        )
      !this.$v.category.categoryMustBeUnique &&
        errors.push('The category must be unique')

      !this.$v.category.required && errors.push('A category is required')
      return errors
    },
    items() {
      return this.$store.state.Category.categories
    },
    busy() {
      return this.$store.state.Category.addCategoryBusy
    }
  },
  watch: {
    items() {
      this.category = ''
      this.$v.$reset()
    }
  }
}
</script>

<style scoped>
</style>
