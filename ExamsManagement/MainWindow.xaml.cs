using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamsManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Examination examination;
        private int selectedId = -1;
        public MainWindow()
        {
            InitializeComponent();
            examination = new Examination();
            LoadMarks();
            UpdateStatistics();
        }

        private void LoadMarks()
        {
            try
            {
                List<Mark> marks = examination.GetAllMarks();
                dgMarks.ItemsSource = marks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading marks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                List<Mark> marks = examination.GetAllMarks();

                txtTotalStudents.Text = marks.Count.ToString();

                if (marks.Count > 0)
                {
                    double average = marks.Average(m => m.MarkValue);
                    txtAverageMark.Text = average.ToString("F1");

                    int highest = marks.Max(m => m.MarkValue);
                    txtHighestMark.Text = highest.ToString();
                }
                else
                {
                    txtAverageMark.Text = "0.0";
                    txtHighestMark.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating statistics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtStudentNumber.Text))
                {
                    MessageBox.Show("Please enter a student number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtStudentNumber.Focus();
                    return;
                }

                if (!int.TryParse(txtMark.Text, out int markValue))
                {
                    MessageBox.Show("Please enter a valid mark (0-100).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtMark.Focus();
                    return;
                }

                if (markValue < 0 || markValue > 100)
                {
                    MessageBox.Show("Mark must be between 0 and 100.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtMark.Focus();
                    return;
                }

                string grade = Grading.GetGrade(markValue);
                examination.AddMark(new Mark
                {
                    StudentNumber = txtStudentNumber.Text.Trim(),
                    MarkValue = markValue,
                    Grade = grade
                });

                MessageBox.Show("Mark added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadMarks();
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding mark: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedId == -1)
                {
                    MessageBox.Show("Please select a record from the grid to update.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtStudentNumber.Text))
                {
                    MessageBox.Show("Please enter a student number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtStudentNumber.Focus();
                    return;
                }

                if (!int.TryParse(txtMark.Text, out int markValue))
                {
                    MessageBox.Show("Please enter a valid mark (0-100).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtMark.Focus();
                    return;
                }

                if (markValue < 0 || markValue > 100)
                {
                    MessageBox.Show("Mark must be between 0 and 100.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtMark.Focus();
                    return;
                }

                string grade = Grading.GetGrade(markValue);
                var markToUpdate = new Mark
                {
                    Id = selectedId,
                    StudentNumber = txtStudentNumber.Text.Trim(),
                    MarkValue = markValue,
                    Grade = grade
                };
                examination.UpdateMark(markToUpdate);

                MessageBox.Show("Mark updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
                LoadMarks();
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating mark: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedId == -1)
                {
                    MessageBox.Show("Please select a record from the grid to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete the record for student '{txtStudentNumber.Text}'?\n\n⚠️ This action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    examination.DeleteMark(selectedId);
                    MessageBox.Show("Mark deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                    LoadMarks();
                    UpdateStatistics();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting mark: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadMarks();
            UpdateStatistics();
            ClearForm();
            MessageBox.Show("Data refreshed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgMarks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMarks.SelectedItem != null)
            {
                Mark selectedMark = (Mark)dgMarks.SelectedItem;
                selectedId = selectedMark.Id;
                txtStudentNumber.Text = selectedMark.StudentNumber;
                txtMark.Text = selectedMark.MarkValue.ToString();
                txtGrade.Text = selectedMark.Grade;
            }
        }

        private void ClearForm()
        {
            selectedId = -1;
            txtStudentNumber.Clear();
            txtMark.Clear();
            txtGrade.Text = string.Empty;
            dgMarks.SelectedItem = null;
            txtStudentNumber.Focus();
        }
    }
}