<template>
    <div id="functionDescriptions">
        <h5>View Available Functions:</h5>

        <b-dropdown text="public new byte unpack_byte(int off)" block split split-variant="outline-secondary" variant="secondary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will return the value of the byte from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new ushort unpack_word(int off)" block split split-variant="outline-secondary"  variant="primary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will unpack a value equal to 2 bytes from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new string unpack_guid(int off)" block split split-variant="outline-secondary"  variant="secondary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will return a GUID value from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new string unpack_wstring(int off, int length = 0)" block split split-variant="outline-secondary" variant="primary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will unpack a Unicode encoded String of various sizing from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new string unpack_string(int off, int length = 0)" block split split-variant="outline-secondary" variant="secondary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will a string of various sizing from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new uint unpack_dword(int off)" block split split-variant="outline-secondary" variant="primary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will unpack a value equivalent to 4 bytes from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new ulong UnpackQword(int off)" block split split-variant="outline-secondary" variant="secondary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will unpack a value equivalent to 8 bytes from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new DateTime unpack_dosdate(int off)" block split split-variant="outline-secondary" variant="primary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will return the date value of 4 bytes from a binary block at the offset specified in the function argument.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new DateTime UnpackFileTime(int off)" block split split-variant="outline-secondary" variant="secondary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will pull out a Windows File time from its byte representation from a binary block at the offset specified in the function argument. FilesTimes are represented in 8 bytes.</b-dropdown-text>
        </b-dropdown>

        <b-dropdown text="public new int align(int off, int alignment)" block split split-variant="outline-secondary" variant="primary" class="m-2" menu-class="w-100">
            <b-dropdown-text>This function will get the offset at a specific alignment value.</b-dropdown-text>
        </b-dropdown>
    </div>
</template>

<script>

    export default {
        name: 'ScriptFunctions'
    }

</script>