<template>
	<div id='items'>
		<b-dropdown-item><router-link to='/SeeShells/register'>Register</router-link></b-dropdown-item>
		<b-dropdown-item><router-link to='/SeeShells/login'>Login</router-link></b-dropdown-item>
	</div>
</template>

<script>
	export default {
		name: 'LoggedOut',
		methods: {

		}
	}
</script>