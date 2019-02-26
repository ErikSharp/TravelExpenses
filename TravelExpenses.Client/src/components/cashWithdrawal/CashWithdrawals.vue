<template>
  <div>
    <v-window touchless v-model="cashWithdrawalWindow">
      <v-window-item>
        <edit-cash-withdrawal edit @done="returnToRecent" />
      </v-window-item>
      <v-window-item>
        <recent-cash-withdrawals
          @addCashWithdrawal="addCashWithdrawal"
          @editCashWithdrawal="editCashWithdrawal()"
        ></recent-cash-withdrawals>
      </v-window-item>
      <v-window-item>
        <edit-cash-withdrawal @done="returnToRecent" />
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import RecentCashWithdrawals from '@/components/cashWithdrawal/RecentCashWithdrawals.vue'
import EditCashWithdrawal from '@/components/cashWithdrawal/EditCashWithdrawal.vue'

export default {
  components: {
    RecentCashWithdrawals,
    EditCashWithdrawal
  },
  data() {
    return {
      cashWithdrawalWindow: 1,
      cashWithdrawalToEdit: {}
    }
  },
  methods: {
    addCashWithdrawal() {
      this.cashWithdrawalWindow = 2
      this.setTitle('Add Cash Withdrawal')
      this.scrollToTop()
    },
    editCashWithdrawal() {
      this.cashWithdrawalWindow = 0
      this.setTitle('Edit Cash Withdrawal')
      this.scrollToTop()
    },
    returnToRecent() {
      this.cashWithdrawalWindow = 1
      this.setTitle('Cash Withdrawals')
      this.scrollToTop()
    },
    setTitle(title) {
      if (this.title !== 'Reconcile') {
        this.$store.dispatch('setTitle', title, { root: true })
      }
    },
    scrollToTop() {
      this.$vuetify.goTo(0, {
        duration: 300,
        offset: 0,
        easing: 'easeInOutCubic'
      })
    }
  },
  computed: {
    title() {
      return this.$store.state.title
    }
  }
}
</script>

<style scoped></style>
