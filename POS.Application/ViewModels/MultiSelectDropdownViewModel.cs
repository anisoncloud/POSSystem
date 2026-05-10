using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace POS.Application.ViewModels
{
    public class MultiSelectDropdownViewModel
    {
        // The HTML input name attribute — e.g. "CategoryIds"
        public string FieldName { get; set; } = default!;
        // Placeholder text shown when nothing is selected
        public string Placeholder { get; set; } = "Select Categories...";
        // All available options to show in dropdown
        public List<SelectOption> Options { get; set; } = new();
        //IDs thats are already selected (used for edit)
        public List<int> SelectedIds { get; set; } = new();
    }
    public class SelectOption
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; } = default!;
        [JsonPropertyName("subText")]
        public string? SubText {  get; set; } //e.g. parent category name

    }
}
