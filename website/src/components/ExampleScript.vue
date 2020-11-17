<template>
    <div id="exampleScript" align="left">
        <h2>Example Script (0x71) </h2>
        <code v-pre>
            local guid = shellitem:unpack_guid(0x0E) <br />
            local flag = shellitem:unpack_byte(0x03) <br />
            <br />
            properties:Add("TypeName", "Control Panel")<br />
            properties:Add("Guid", tostring(guid))<br />
            properties:Add("Flags", tostring(flag))<br />
            <br />
            if knownGUIDs:ContainsKey(tostring(guid)) then<br />
            properties:Add("Name", string.format("{{CONTROL PANEL: %s}}", knownGUIDs[guid]))<br />
            else<br />
            properties:Add("Name", string.format("{{CONTROL PANEL: %s}}", guid))<br />
            <br />
            end<br />
        </code>
        <br />
        <h4>Script Explanation</h4>
        <p>
            This script parses a Control Panel shell item (hex identifier: 0x71). Scripts must be written in Lua and can use some C# functions through the NLua library.
            The "shellitem" object is the binary shellbag data that must be parsed.
            The "properties" object holds the information that will be returned; an item must be added to "properties" for the script to be useable.
            Add additional items to "properties" to provide more information. Whatever is held in properties is what users will see about the shell item.
            "knownGUIDs" is a dictionary where the key is a GUID and the value is a human-readable string of that GUID.
            Some C# functions are made available in the program to aid in parsing shellbag data. See the available functions below. You can use them by
            typing shellitem:[function]() where [function] is the function name like unpack_guid.
        </p>
        <a href="https://www.lua.org/docs.html">For help with writing Lua scripts, look here.</a>
        <br />
        <a href="http://nlua.org/">For more information on NLua, look here.</a>
        <br />
        <br />
        <ScriptFunctions />
    </div>
</template>

<script>
    import ScriptFunctions from './AvailableScriptFunctions.vue';

    export default {
        name: 'ExampleScript',
        components: {ScriptFunctions}
    }
</script>

<style>
    h2{
        text-align:center;
    }
    h3{
        text-align:center;
    }
    h4{
        text-align:center;
    }
    p{
        text-align:justify;
    }

</style>